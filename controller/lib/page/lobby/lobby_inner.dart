import 'package:controller/page/lobby/bloc/lobby_cubit.dart';
import 'package:controller/widget/button_form_wrapper.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class LobbyInner extends StatelessWidget {
  const LobbyInner({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    final cubit = context.watch<LobbyCubit>();
    final state = cubit.state;

    final content = CustomScrollView(
      slivers: [
        SliverAppBar(
          title: const Text('Lobby'),
          pinned: true,
        ),
        SliverToBoxAdapter(
          child: Wrap(
            children: [
              for (final user in state.users)
                Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: Chip(
                    label: Text(user.displayName),
                    backgroundColor: user.color,
                  ),
                ),
            ],
          ),
        ),
      ],
    );

    return Scaffold(
      body: ButtonFormWrapper(
        primaryButton: ElevatedButton(
          onPressed: () {
            cubit.startGame();
          },
          child: const Text('Start Game!'),
        ),
        child: content,
      ),
    );
  }
}
