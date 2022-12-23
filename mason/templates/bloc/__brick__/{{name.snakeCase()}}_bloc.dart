import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:bloc_concurrency/bloc_concurrency.dart';

part '{{name.snakeCase()}}_state.dart';
part '{{name.snakeCase()}}_event.dart';
part '{{name.snakeCase()}}_bloc.freezed.dart';

class {{name.pascalCase()}}Bloc extends Bloc<{{name.pascalCase()}}Event, {{name.pascalCase()}}State> {
  {{name.pascalCase()}}Bloc() : super({{name.pascalCase()}}State()) {
    on<{{name.pascalCase()}}Event>(
      (event, emit) async {},
      transformer: sequential(),
    );
  }
}
