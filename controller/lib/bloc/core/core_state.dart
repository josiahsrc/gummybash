// ignore_for_file: invalid_annotation_target

part of 'core_cubit.dart';

@freezed
class CoreState with _$CoreState {
  const factory CoreState.setup({
    String? errorMessage,
  }) = SetupState;

  const factory CoreState.ready({
    required String userId,
    required GameState gameState,
    String? errorMessage,
  }) = ReadyState;
}

extension ReadyStateX on ReadyState {
  User? get user =>
      gameState.users.where((element) => element.id == userId).first;
}
