import 'package:controller/bloc/core/core_bloc.dart';
import 'package:controller/repo/server_repo.dart';
import 'package:flutter/material.dart';
import 'package:logger/logger.dart';

import 'app.dart';

void main() {
  Logger.level = Level.verbose;

  final serverRepo = ServerRepo();
  final coreBloc = CoreBloc();

  runApp(App(
    serverRepo: serverRepo,
    coreBloc: coreBloc,
  ));
}
