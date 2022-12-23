import 'package:controller/bloc/core/core_cubit.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'bloc/join_cubit.dart';
import 'join_forms.dart';

class JoinPage extends StatelessWidget {
  const JoinPage({super.key});

  @override
  Widget build(BuildContext context) {
    final composedForm = BlocProvider(
      create: (context) => JoinCubit(
        coreCubit: context.read<CoreCubit>(),
      ),
      child: JoinForms(),
    );

    return Scaffold(
      body: composedForm,
    );
  }
}
