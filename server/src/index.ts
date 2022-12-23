import { server as WSServer } from "websocket";
import * as http from "http";

function cloneGameState(state: GameState): GameState {
  return {
    users: state.users.map(user => ({
      id: user.id,
      displayName: user.displayName,
      updatedAt: user.updatedAt,
      joystickX: user.joystickX,
      joystickY: user.joystickY,
      buttonPresses: user.buttonPresses,
    })),
    updatedAt: state.updatedAt,
    startTimestamp: state.startTimestamp,
    endTimestamp: state.endTimestamp,
    lobbyTimestamp: state.lobbyTimestamp,
    winnerId: state.winnerId,
  };
}

const PORT = 8080;
const KICK_LATENCY = 1000 * 5;
const GAME_DURATION = 1000 * 60 * 2;
const CREDITS_DURATION = 1000 * 8;

const server = http.createServer();
server.listen(PORT);
const wss = new WSServer({
  httpServer: server,
});

const state: GameState = {
  users: [],
  updatedAt: new Date().toISOString(),
  lobbyTimestamp: new Date().toISOString(),
};
let lastState = cloneGameState(state);

// Handle requests
wss.on('request', function (request) {
  const connection = request.accept(null, request.origin);
  console.log('Connection accepted');

  connection.on('message', function (raw) {
    if (raw.type !== 'utf8') {
      console.error('Received non-utf8 message');
      return;
    }

    let msg: any;
    try {
      msg = JSON.parse(raw.utf8Data);
    } catch (e) {
      console.error('Received invalid JSON', raw.utf8Data);
      return;
    }

    const type = msg.runtimeType;
    console.log('Received message', msg);
    if (!type) {
      console.error('Received message without runtimeType');
      return;
    }

    if (type === 'joinGame') {
      const req = msg as JoinGameRequest;
      const user: User = {
        id: req.userId,
        displayName: req.displayName,
        updatedAt: new Date().toISOString(),
        joystickX: 0,
        joystickY: 0,
        buttonPresses: 0,
      };

      state.users.push(user);
      state.updatedAt = new Date().toISOString();
      console.log('User joined: ', user);
    } else if (type === 'updateUser') {
      const req = msg as UpdateUserRequest;
      const user = state.users.find(user => user.id === req.userId);
      if (!user) {
        console.error('Received update for unknown user');
        return;
      }

      user.joystickX = req.joystickX;
      user.joystickY = req.joystickY;
      if (req.buttonPressed) {
        user.buttonPresses++;
      }
      user.updatedAt = new Date().toISOString();
      state.updatedAt = new Date().toISOString();
    } else if (type === 'updateGameState') {
      const req = msg as UpdateGameStateRequest;
      if (req.start) {
        const time = new Date().getTime();
        state.startTimestamp = new Date(time).toISOString();
        state.endTimestamp = new Date(time + GAME_DURATION).toISOString();
        state.lobbyTimestamp = new Date(time + GAME_DURATION + CREDITS_DURATION).toISOString();
        state.winnerId = undefined;
      } else if (req.lobby) {
        state.startTimestamp = undefined;
        state.endTimestamp = undefined;
        state.lobbyTimestamp = new Date().toISOString();
        state.winnerId = undefined;
      } else if (req.winnerId) {
        if (!state.startTimestamp) {
          console.error('Received winner before game started');
          return;
        }
        const time = new Date().getTime();
        state.winnerId = req.winnerId;
        state.endTimestamp = new Date(time).toISOString();
        state.lobbyTimestamp = new Date(time + CREDITS_DURATION).toISOString();
      }
    } else {
      console.error('Received unknown message type', type);
    }
  });
});

// Sync game state
setInterval(() => {
  const now = new Date().getTime();

  // // Kick inactive users
  // state.users = state.users.filter(user => {
  //   const timeDiff = now - new Date(user.updatedAt).getTime();
  //   return timeDiff < KICK_LATENCY;
  // });
  // state.updatedAt = new Date().toISOString();

  // End game if time is up
  const lobbyTimeDiff = now - new Date(state.lobbyTimestamp).getTime();
  if (lobbyTimeDiff > 0) {
    state.startTimestamp = undefined;
    state.endTimestamp = undefined;
    state.winnerId = undefined;
  }

  // Send game state to all clients
  wss.broadcast(JSON.stringify(<GameStateResponse>{
    runtimeType: 'gameState',
    gameState: state,
  }));

  // check if state changed (except for updatedAt)
  const curr = cloneGameState(state);
  const last = cloneGameState(lastState);
  curr.updatedAt = last.updatedAt;
  if (JSON.stringify(curr) !== JSON.stringify(last)) {
    console.log('state changed', state);
  }

  // update last state
  lastState = cloneGameState(state);
}, 5);

console.log(`Server started on port ${PORT}`);
