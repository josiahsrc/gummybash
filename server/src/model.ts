export enum UserType {
  none = 'none',
  gummyBear = 'gummyBear',
  gingerBreadHouse = 'gingerBreadHouse',
}

export interface User {
  id: string;
  displayName: string;
  updatedAt: string;
  joystickX: number;
  joystickY: number;
  buttonPresses: number;
  color: string;
  type: UserType;
}

export interface GameState {
  users: User[];
  updatedAt: string;
  startTimestamp?: string;
  endTimestamp?: string;
  lobbyTimestamp: string;
  winner: UserType;
  preStart: boolean;
}

export interface JoinGameRequest {
  runtimeType: 'joinGame';
  displayName: string;
  userId: string;
}

export interface UpdateUserRequest {
  runtimeType: 'updateUser';
  userId: string;
  joystickX?: number;
  joystickY?: number;
  buttonPressed?: boolean;
}

export interface UpdateGameStateRequest {
  runtimeType: 'updateGameState';
  winner?: UserType;
  preStart?: boolean;
  start?: boolean;
  lobby?: boolean;
}

export interface GameStateResponse {
  runtimeType: 'gameState';
  gameState: GameState;
}

export interface ErrorResponse {
  runtimeType: 'error';
  message: string;
}
