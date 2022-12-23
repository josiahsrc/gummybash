import 'package:logger/logger.dart';

class CondensedLogPrinter extends LogPrinter {
  CondensedLogPrinter({required this.name});

  final String name;

  @override
  List<String> log(LogEvent event) {
    final now = DateTime.now();
    final level = event.level.name;
    final msg = event.message;

    if (event.error == null) {
      return ['[$level][$now] $msg'];
    } else {
      return ['[$level][$now] $msg - [${event.error}]'];
    }
  }
}

class LoggingUtility {
  const LoggingUtility._();

  static Logger createNamedLogger(String name) => Logger(
        printer: CondensedLogPrinter(name: name),
      );

  static Logger createTypedLogger(Type type) =>
      createNamedLogger(type.toString());
}
