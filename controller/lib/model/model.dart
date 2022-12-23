// ignore_for_file: invalid_annotation_target

import 'package:controller/utility/json.dart';
import 'package:flutter/material.dart';
import 'package:freezed_annotation/freezed_annotation.dart';

part 'model.freezed.dart';
part 'model.g.dart';

@freezed
class User with _$User {
  const factory User({
    required String id,
    required String displayName,
    required DateTime updatedAt,
    required double joystickX,
    required double joystickY,
    required int buttonPresses,
    @JsonKey(
      fromJson: hexToColor,
      toJson: colorToHex,
    )
    required Color color,
  }) = _User;

  factory User.fromJson(Map<String, dynamic> json) => _$UserFromJson(json);
}

@freezed
class GameState with _$GameState {
  const factory GameState({
    required List<User> users,
    required DateTime updatedAt,
    @JsonKey(
      fromJson: maybeDateTimeFromJson,
      toJson: maybeDateTimeToJson,
    )
    required DateTime? startTimestamp,
    @JsonKey(
      fromJson: maybeDateTimeFromJson,
      toJson: maybeDateTimeToJson,
    )
    required DateTime? endTimestamp,
    required DateTime lobbyTimestamp,
    required String? lastWinnerId,
  }) = _GameState;

  factory GameState.fromJson(Map<String, dynamic> json) =>
      _$GameStateFromJson(json);
}

@freezed
class Requests with _$Requests {
  const factory Requests.joinGame({
    required String displayName,
    required String userId,
  }) = JoinGameRequest;

  const factory Requests.updateUser({
    required String userId,
    required double joystickX,
    required double joystickY,
    required bool buttonPressed,
  }) = UpdateUserRequest;

  const factory Requests.updateGameState({
    required String? winnerId,
    required bool? start,
    required bool? lobby,
  }) = UpdateGameStateRequest;

  factory Requests.fromJson(Map<String, dynamic> json) =>
      _$RequestsFromJson(json);
}

@freezed
class Responses with _$Responses {
  const factory Responses.gameState({
    required GameState gameState,
  }) = GameStateResponse;

  const factory Responses.error({
    required String message,
  }) = ErrorResponse;

  factory Responses.fromJson(Map<String, dynamic> json) =>
      _$ResponsesFromJson(json);
}
