import 'package:controller/utility/logging.dart';

class ServerException implements Exception {}

class ServerRepo {
  final logger = LoggingUtility.createTypedLogger(ServerRepo);
}
