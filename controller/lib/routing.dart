import 'package:auto_route/auto_route.dart';
import 'package:controller/page/lobby/lobby_page.dart';
import 'package:controller/page/pre_start/pre_start_page.dart';
import 'package:flutter/material.dart';
import 'package:controller/bloc/core/core_cubit.dart';
import 'package:controller/page/join/join_page.dart';
import 'package:controller/page/play/play_page.dart';

part 'routing.gr.dart';

@MaterialAutoRouter(
  replaceInRouteName: 'Page,Route',
  routes: <AutoRoute>[
    CustomRoute(
      path: '/join',
      page: JoinPage,
      initial: true,
      transitionsBuilder: TransitionsBuilders.fadeIn,
    ),
    CustomRoute(
      path: '/play',
      page: PlayPage,
      transitionsBuilder: TransitionsBuilders.fadeIn,
      guards: [CoreStateGuard],
    ),
    CustomRoute(
      path: '/lobby',
      page: LobbyPage,
      transitionsBuilder: TransitionsBuilders.fadeIn,
      guards: [CoreStateGuard],
    ),
    CustomRoute(
      path: '/pre-start',
      page: PreStartPage,
      transitionsBuilder: TransitionsBuilders.fadeIn,
      guards: [CoreStateGuard],
    ),
  ],
)
class AppRouter extends _$AppRouter {
  AppRouter({
    required super.coreStateGuard,
  });
}

PageRouteInfo getRouteForCoreState(CoreState state) {
  if (state is SetupState) {
    return JoinRoute();
  } else if (state is ReadyState) {
    if (state.isInLobby) {
      if (state.gameState.preStart) {
        return PreStartRoute();
      } else {
        return LobbyRoute();
      }
    } else {
      return PlayRoute();
    }
  } else {
    throw UnimplementedError(state.runtimeType.toString());
  }
}

class CoreStateGuard extends AutoRouteGuard {
  CoreStateGuard({
    required this.coreCubit,
  });

  final CoreCubit coreCubit;

  @override
  void onNavigation(
    NavigationResolver resolver,
    StackRouter router,
  ) {
    final state = coreCubit.state;
    final route = getRouteForCoreState(state);
    if (route.path == resolver.route.path) {
      resolver.next(true);
    } else {
      router.replaceAll([JoinRoute()]);
      resolver.next(false);
    }
  }
}
