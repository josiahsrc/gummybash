import 'package:controller/bloc/core/core_cubit.dart';
import 'package:controller/repo/server_repo.dart';
import 'package:flutter/material.dart';
import 'package:logger/logger.dart';

import 'app.dart';

void main() {
  WidgetsFlutterBinding.ensureInitialized();

  Logger.level = Level.verbose;

  final serverRepo = ServerRepo();
  final coreBloc = CoreCubit()..init();

  return runApp(App(
    serverRepo: serverRepo,
    coreCubit: coreBloc,
  ));
}
