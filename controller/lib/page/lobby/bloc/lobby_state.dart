// ignore_for_file: invalid_annotation_target

part of 'lobby_cubit.dart';

@freezed
class LobbyState with _$LobbyState {
  const factory LobbyState({
    @Default([]) List<User> users,
  }) = _LobbyState;
}
