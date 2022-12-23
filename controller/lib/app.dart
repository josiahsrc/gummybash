import 'package:controller/bloc/core/core_cubit.dart';
import 'package:controller/repo/server_repo.dart';
import 'package:controller/routing.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class App extends StatefulWidget {
  const App({
    super.key,
    required this.serverRepo,
    required this.coreCubit,
  });

  final ServerRepo serverRepo;
  final CoreCubit coreCubit;

  @override
  State<App> createState() => _AppState();
}

class _AppState extends State<App> {
  late final AppRouter appRouter;

  @override
  void initState() {
    WidgetsBinding.instance.addPostFrameCallback((_) {
      syncRouterState(widget.coreCubit.state);
    });

    appRouter = AppRouter(
      coreStateGuard: CoreStateGuard(coreCubit: widget.coreCubit),
    );

    super.initState();
  }

  void syncRouterState(CoreState state) {
    final route = getRouteForCoreState(state);
    appRouter.replaceAll([route]);
  }

  @override
  Widget build(BuildContext context) {
    final materialApp = MaterialApp.router(
      routerDelegate: appRouter.delegate(),
      routeInformationParser: appRouter.defaultRouteParser(),
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
            listener: (context, state) => syncRouterState(state),
          ),
        ],
        child: materialApp,
      ),
    );

    final providers = MultiRepositoryProvider(
      providers: [
        RepositoryProvider.value(value: widget.serverRepo),
      ],
      child: MultiBlocProvider(
        providers: [
          BlocProvider.value(value: widget.coreCubit),
        ],
        child: listeners,
      ),
    );

    return providers;
  }
}
