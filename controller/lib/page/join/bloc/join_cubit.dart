import 'dart:async';

import 'package:controller/bloc/core/core_cubit.dart';
import 'package:controller/utility/form.dart';
import 'package:controller/utility/logging.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

part 'join_state.dart';
part 'join_cubit.freezed.dart';

class JoinCubit extends Cubit<JoinState> {
  JoinCubit({
    required this.coreCubit,
  }) : super(JoinState());

  final CoreCubit coreCubit;

  final logger = LoggingUtility.createTypedLogger(JoinCubit);

  void submitTitleForm() {
    emit(state.copyWith(titleSubmission: SubmissionStatus.finished));
  }

  void unsubmitTitleForm() {
    emit(state.copyWith(titleSubmission: SubmissionStatus.ready));
  }

  void updateDisplayName(String value) {
    String? validation;
    if (value.length < 2) {
      validation = 'Must be at least 2 characters.';
    } else if (value.length > 20) {
      validation = 'Must be less than 20 characters.';
    }

    emit(state.copyWith(
      displayName: value,
      displayNameMessage: validation,
      displayNameSubmission: validation == null
          ? SubmissionStatus.ready
          : SubmissionStatus.blocked,
    ));
  }

  Future<void> submitDisplayNameForm() async {
    emit(state.copyWith(displayNameSubmission: SubmissionStatus.ready));
    try {
      emit(state.copyWith(displayNameSubmission: SubmissionStatus.inProgress));
      logger.i('joining game with display name: ${state.displayName}');
      await coreCubit.joinGame(displayName: state.displayName);
      logger.i('successfully joined game');
      emit(state.copyWith(displayNameSubmission: SubmissionStatus.finished));
    } catch (e) {
      logger.e('Failed to update user prefs', e);
      emit(state.copyWith(
        popupErrorMessage: 'Something went wrong',
        displayNameSubmission: SubmissionStatus.ready,
      ));
    }
  }
}
