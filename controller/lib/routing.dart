import 'package:controller/page/home/home_page.dart';
import 'package:go_router/go_router.dart';

final router = GoRouter(
  routes: [
    GoRoute(
      path: '/',
      builder: (context, state) => const HomePage(),
    ),
  ],
  // redirect: (context, state) {
  //   final core = BlocProvider.of<CoreBloc>(context, listen: false).state;
  //   return getRouteForCoreState(core);
  // },
);
