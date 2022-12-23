import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

part 'play_state.dart';
part 'play_cubit.freezed.dart';

class PlayCubit extends Cubit<PlayState> {
  PlayCubit() : super(PlayState());
}
