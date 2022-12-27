import 'package:controller/page/pre_start/pre_start_inner.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../../bloc/core/core_cubit.dart';
import 'bloc/pre_start_cubit.dart';

class PreStartPage extends StatelessWidget {
  const PreStartPage({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => PreStartCubit(
        coreCubit: context.read<CoreCubit>(),
      )..init(),
      child: PreStartInner(),
    );
  }
}
