import 'package:controller/bloc/core/core_cubit.dart';
import 'package:controller/page/join/join_page.dart';
import 'package:controller/page/play/play_page.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';

final router = GoRouter(
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => JoinPage(),
    ),
    GoRoute(
      path: '/play',
      builder: (context, state) => PlayPage(),
    ),
  ],
  redirect: (context, state) {
    final core = BlocProvider.of<CoreCubit>(context, listen: false).state;
    return getRouteForCoreState(core);
  },
);

String getRouteForCoreState(CoreState state) {
  if (state is SetupState) {
    return '/';
  } else if (state is ReadyState) {
    return '/play';
  } else {
    throw UnimplementedError(state.runtimeType.toString());
  }
}
