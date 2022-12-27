// ignore_for_file: invalid_annotation_target

part of 'play_cubit.dart';

@freezed
class PlayState with _$PlayState {
  const factory PlayState({
    String? timeRemaining,
    String? winnerText,
    Color? color,
    @Default(UserType.none) UserType type,
    @Default(UserType.none) UserType winner,
  }) = _PlayState;
}
