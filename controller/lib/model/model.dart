// ignore_for_file: invalid_annotation_target

import 'package:controller/utility/json.dart';
import 'package:flutter/material.dart';
import 'package:freezed_annotation/freezed_annotation.dart';

part 'model.freezed.dart';
part 'model.g.dart';

enum UserType {
  none,
  gummyBear,
  gingerBreadHouse,
}

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
    @Default(UserType.none)
        UserType type,
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
    required bool preStart,
    @Default(UserType.none)
        UserType winner,
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
    double? joystickX,
    double? joystickY,
    bool? buttonPressed,
  }) = UpdateUserRequest;

  const factory Requests.updateGameState({
    required UserType? winner,
    required bool? start,
    required bool? lobby,
    required bool? preStart,
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
