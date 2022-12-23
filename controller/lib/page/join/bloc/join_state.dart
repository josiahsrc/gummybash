// ignore_for_file: invalid_annotation_target

part of 'join_cubit.dart';

@freezed
class JoinState with _$JoinState {
  const factory JoinState({
    String? popupErrorMessage,

    // Title
    @Default(SubmissionStatus.ready) SubmissionStatus titleSubmission,

    // Display Name
    @Default(SubmissionStatus.ready) SubmissionStatus displayNameSubmission,
    @Default('test') String displayName,
    String? displayNameMessage,
  }) = _JoinState;

  // ignore: unused_element
  const JoinState._();

  int get currentPage => formIndexFromSubmissions([
        titleSubmission,
        displayNameSubmission,
      ]);
}
