import 'package:controller/model/model.dart';
import 'package:controller/page/pre_start/bloc/pre_start_cubit.dart';
import 'package:controller/widget/button_form_wrapper.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class PreStartInner extends StatelessWidget {
  const PreStartInner({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    final cubit = context.watch<PreStartCubit>();
    final state = cubit.state;

    if (state.user == null) {
      return const Scaffold(
        body: Center(
          child: CircularProgressIndicator(),
        ),
      );
    }
    final content = CustomScrollView(
      slivers: [
        if (state.user!.type == UserType.gingerBreadHouse) ...[
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Image.asset('assets/Instructions_GH1.jpg'),
            ),
          ),
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Image.asset('assets/Instructions_GH2_Goal.jpg'),
            ),
          ),
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Image.asset('assets/Instructions_GH3_Gameplay.jpg'),
            ),
          ),
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Image.asset('assets/Instructions_GH4_Winning.jpg'),
            ),
          ),
        ],
        if (state.user!.type == UserType.gummyBear) ...[
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Stack(
                children: [
                  Image.asset('assets/Instructions_GB1.jpg'),
                  Positioned.fill(
                    child: Align(
                      alignment: Alignment(0, -.2),
                      child: SizedBox.square(
                        dimension: MediaQuery.of(context).size.width * 0.4,
                        child: Image.asset(
                          'assets/gummy-bear-icon.png',
                          color: state.user!.color,
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Image.asset('assets/Instructions_GB2_Goal.jpg'),
            ),
          ),
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Image.asset('assets/Instructions_GB3_Gameplay.jpg'),
            ),
          ),
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Image.asset('assets/Instructions_GB4_IFworm.jpg'),
            ),
          ),
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Image.asset('assets/Instructions_GB5_Winning.jpg'),
            ),
          ),
        ],
        SliverToBoxAdapter(
          child: SizedBox(
            height: 180,
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
        child: SafeArea(child: content),
      ),
    );
  }
}
