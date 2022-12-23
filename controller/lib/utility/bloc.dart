import 'package:flutter_bloc/flutter_bloc.dart';

extension BlocBaseX<State> on BlocBase<State> {
  Stream<State> get streamWithFirst {
    Stream<State> internal() async* {
      yield state;
      yield* stream;
    }

    return internal();
  }
}
