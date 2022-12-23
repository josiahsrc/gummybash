import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../../bloc/core/core_cubit.dart';
import 'bloc/lobby_cubit.dart';
import 'lobby_inner.dart';

class LobbyPage extends StatelessWidget {
  const LobbyPage({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => LobbyCubit(
        coreCubit: context.read<CoreCubit>(),
      )..init(),
      child: LobbyInner(),
    );
  }
}
