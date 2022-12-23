import 'dart:convert';

import 'package:controller/model/model.dart';
import 'package:controller/utility/logging.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:web_socket_channel/web_socket_channel.dart';
import 'package:web_socket_channel/status.dart' as status;
import 'package:uuid/uuid.dart';

part 'core_state.dart';
part 'core_cubit.freezed.dart';

const port = '8080';
const host = 'localhost';

class CoreCubit extends Cubit<CoreState> {
  CoreCubit() : super(CoreState.setup());

  final logger = LoggingUtility.createTypedLogger(CoreCubit);
  WebSocketChannel? _channel;
  String? _userId;

  bool _isReady() {
    if (_userId == null) {
      return false;
    }

    if (_channel == null) {
      // logger.e('channel is null');
      return false;
    }

    if (state is! ReadyState) {
      // logger.e('state is not ReadyState');
      return false;
    }

    return true;
  }

  void init() {
    _channel?.sink.close(status.goingAway);
    _channel = WebSocketChannel.connect(Uri.parse('ws://$host:$port'));
    _channel!.stream.listen(_handleMessage);
  }

  Future<void> _addMessage(dynamic message) async {
    final utf8Message = jsonEncode(message.toJson());
    _channel?.sink.add(utf8Message);
  }

  Future<void> startGame() async {
    if (!_isReady()) {
      return;
    }

    return _addMessage(
      Requests.updateGameState(
        start: true,
        lobby: false,
        winnerId: null,
      ),
    );
  }

  Future<void> returnToLobby() async {
    if (!_isReady()) {
      return;
    }

    return _addMessage(
      Requests.updateGameState(
        start: false,
        lobby: true,
        winnerId: null,
      ),
    );
  }

  Future<void> joinGame({
    required String displayName,
  }) async {
    _userId = Uuid().v4();
    return _addMessage(
      Requests.joinGame(
        displayName: displayName,
        userId: _userId!,
      ),
    );
  }

  Future<void> updateUser({
    required double joystickX,
    required double joystickY,
    required bool buttonPressed,
  }) async {
    if (!_isReady()) {
      return;
    }

    return _addMessage(
      Requests.updateUser(
        userId: _userId!,
        joystickX: joystickX,
        joystickY: joystickY,
        buttonPressed: buttonPressed,
      ),
    );
  }

  void _handleMessage(dynamic message) {
    try {
      if (message is! String) {
        logger.e('Received non-string message: $message');
        return;
      }

      final json = jsonDecode(message);
      final response = Responses.fromJson(json);
      if (response is GameStateResponse) {
        if (_userId == null) {
          return;
        }

        emit(CoreState.ready(
          userId: _userId!,
          gameState: response.gameState,
        ));
      } else if (response is ErrorResponse) {
        logger.e('Received error response: ${response.message}');
      } else {
        logger.e('Received unknown response: $response');
      }
    } catch (e, stackTrace) {
      logger.e('Error handling message: $message', e, stackTrace);
    }
  }

  @override
  Future<void> close() async {
    await _channel?.sink.close(status.goingAway);
    return super.close();
  }
}
