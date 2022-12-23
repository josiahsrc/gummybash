interface User {
  id: string;
  displayName: string;
  updatedAt: string;
  joystickX: number;
  joystickY: number;
  buttonPresses: number;
}

interface GameState {
  users: User[];
  updatedAt: string;
  startTimestamp?: string;
  endTimestamp?: string;
  lobbyTimestamp: string;
  winnerId?: string;
}

interface JoinGameRequest {
  runtimeType: 'joinGame';
  displayName: string;
  userId: string;
}

interface UpdateUserRequest {
  runtimeType: 'updateUser';
  userId: string;
  joystickX: number;
  joystickY: number;
  buttonPressed: boolean;
}

interface UpdateGameStateRequest {
  runtimeType: 'updateGameState';
  winnerId?: string;
  start?: boolean;
  lobby?: boolean;
}

interface GameStateResponse {
  runtimeType: 'gameState';
  gameState: GameState;
}

interface ErrorResponse {
  runtimeType: 'error';
  message: string;
}
