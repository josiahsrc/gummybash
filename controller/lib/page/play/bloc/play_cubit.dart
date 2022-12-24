import 'dart:async';

import 'package:controller/bloc/core/core_cubit.dart';
import 'package:controller/utility/bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

part 'play_state.dart';
part 'play_cubit.freezed.dart';

class PlayCubit extends Cubit<PlayState> {
  PlayCubit({
    required this.coreCubit,
  }) : super(PlayState());

  final CoreCubit coreCubit;
  StreamSubscription? _coreCubitSubscription;

  void init() {
    _coreCubitSubscription = coreCubit.streamWithFirst.listen((state) {
      if (state is! ReadyState || state.isInLobby) {
        emit(this.state.copyWith(timeRemaining: null));
        return;
      }

      final now = DateTime.now();
      final endTime = state.gameState.endTimestamp!;
      final timeRemaining = endTime.difference(now).inSeconds;

      final prettyTimeRemaining = timeRemaining > 0
          ? '${timeRemaining ~/ 60}:${(timeRemaining % 60).toString().padLeft(2, '0')}'
          : '0:00';

      emit(this.state.copyWith(timeRemaining: prettyTimeRemaining));
    });
  }

  void updateControls({
    double joystickX = 0,
    double joystickY = 0,
    bool isShooting = false,
  }) {
    coreCubit.updateUser(
      joystickX: joystickX,
      joystickY: joystickY,
      buttonPressed: isShooting,
    );
  } 

  @override
  Future<void> close() async {
    await _coreCubitSubscription?.cancel();
    return super.close();
  }
}
