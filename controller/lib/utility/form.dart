enum SubmissionStatus {
  blocked,
  ready,
  inProgress,
  finished,
}

int formIndexFromSubmissions(List<SubmissionStatus> submissions) {
  return submissions.indexWhere((s) => s != SubmissionStatus.finished);
}
