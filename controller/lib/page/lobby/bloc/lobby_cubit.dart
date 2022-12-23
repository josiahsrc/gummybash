import 'dart:async';

import 'package:controller/bloc/core/core_cubit.dart';
import 'package:controller/model/model.dart';
import 'package:controller/utility/bloc.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

part 'lobby_state.dart';
part 'lobby_cubit.freezed.dart';

class LobbyCubit extends Cubit<LobbyState> {
  LobbyCubit({
    required this.coreCubit,
  }) : super(LobbyState());

  final CoreCubit coreCubit;
  StreamSubscription? _coreCubitSubscription;

  void init() {
    coreCubit.streamWithFirst.listen((state) {
      if (state is! ReadyState) {
        return;
      }
      emit(this.state.copyWith(users: state.gameState.users));
    });
  }

  Future<void> startGame() async {
    await coreCubit.startGame();
  }

  @override
  Future<void> close() async {
    await _coreCubitSubscription?.cancel();
    return super.close();
  }
}
