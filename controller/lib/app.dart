import 'package:controller/bloc/core/core_cubit.dart';
import 'package:controller/repo/server_repo.dart';
import 'package:controller/routing.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class App extends StatelessWidget {
  const App({
    super.key,
    required this.serverRepo,
    required this.coreCubit,
  });

  final ServerRepo serverRepo;
  final CoreCubit coreCubit;

  @override
  Widget build(BuildContext context) {
    final materialApp = MaterialApp.router(
      routerConfig: router,
      supportedLocales: [
        Locale('en'),
      ],
    );

    final listeners = Builder(
      builder: (context) => MultiBlocListener(
        listeners: [
          BlocListener<CoreCubit, CoreState>(
            listenWhen: (previous, current) {
              final prev = getRouteForCoreState(previous);
              final curr = getRouteForCoreState(current);
              return prev != curr;
            },
            listener: (context, state) {
              final route = getRouteForCoreState(state);
              router.pushReplacementNamed(route);
            },
          ),
        ],
        child: materialApp,
      ),
    );

    final providers = MultiRepositoryProvider(
      providers: [
        RepositoryProvider.value(value: serverRepo),
      ],
      child: MultiBlocProvider(
        providers: [
          BlocProvider.value(value: coreCubit),
        ],
        child: listeners,
      ),
    );

    return providers;
  }
}
