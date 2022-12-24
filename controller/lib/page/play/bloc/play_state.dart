// ignore_for_file: invalid_annotation_target

part of 'play_cubit.dart';

@freezed
class PlayState with _$PlayState {
  const factory PlayState({
    String? timeRemaining,
  }) = _PlayState;
}
