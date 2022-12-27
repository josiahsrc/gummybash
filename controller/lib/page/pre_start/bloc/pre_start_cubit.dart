import 'dart:async';

import 'package:controller/bloc/core/core_cubit.dart';
import 'package:controller/model/model.dart';
import 'package:controller/utility/bloc.dart';
import 'package:flutter/material.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

part 'pre_start_state.dart';
part 'pre_start_cubit.freezed.dart';

class PreStartCubit extends Cubit<PreStartState> {
  PreStartCubit({
    required this.coreCubit,
  }) : super(PreStartState());

  final CoreCubit coreCubit;
  StreamSubscription? _coreCubitSubscription;

  void init() {
    _coreCubitSubscription = coreCubit.streamWithFirst.listen((state) {
      if (state is! ReadyState) {
        return;
      }
      emit(this.state.copyWith(user: state.user));
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
