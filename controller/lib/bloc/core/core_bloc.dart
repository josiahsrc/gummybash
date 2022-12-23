import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bloc_concurrency/bloc_concurrency.dart';

part 'core_state.dart';
part 'core_event.dart';
part 'core_bloc.freezed.dart';

class CoreBloc extends Bloc<CoreEvent, CoreState> {
  CoreBloc() : super(CoreState()) {
    on<CoreEvent>(
      (event, emit) async {},
      transformer: sequential(),
    );
  }
}
