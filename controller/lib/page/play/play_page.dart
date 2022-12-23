import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'bloc/play_cubit.dart';
import 'play_inner.dart';

class PlayPage extends StatelessWidget {
  const PlayPage({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => PlayCubit(),
      child: PlayInner(),
    );
  }
}
