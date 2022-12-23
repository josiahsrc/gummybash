import 'package:controller/bloc/core/core_bloc.dart';
import 'package:controller/repo/server_repo.dart';
import 'package:controller/routing.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class App extends StatelessWidget {
  const App({
    super.key,
    required this.serverRepo,
    required this.coreBloc,
  });

  final ServerRepo serverRepo;
  final CoreBloc coreBloc;

  @override
  Widget build(BuildContext context) {
    Widget app;

    app = MaterialApp.router(
      routerConfig: router,
      supportedLocales: [
        Locale('en'),
      ],
    );

    app = MultiRepositoryProvider(
      providers: [
        RepositoryProvider.value(value: serverRepo),
      ],
      child: MultiBlocProvider(
        providers: [
          BlocProvider.value(value: coreBloc),
        ],
        child: app,
      ),
    );

    return app;
  }
}
