import 'package:controller/utility/form.dart';
import 'package:controller/widget/paged_form.dart';
import 'package:controller/widget/simple_form_layout.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'bloc/join_cubit.dart';

class JoinForms extends StatefulWidget {
  const JoinForms({super.key});

  @override
  State<JoinForms> createState() => _JoinFormsState();
}

class _JoinFormsState extends State<JoinForms> {
  final controller = PagedFormController();

  @override
  void dispose() {
    controller.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final cubit = BlocProvider.of<JoinCubit>(context, listen: true);
    final state = cubit.state;

    int formId = 0;
    int nextFormId() => formId++;

    final titleForm = SimpleFormLayout(
      key: ValueKey(nextFormId()),
      title: const Text('GUMMYBASH'),
      description: const Text(
        'Revenge of the Ginger Bread House',
      ),
      primaryButton: ElevatedButton(
        onPressed: state.titleSubmission == SubmissionStatus.ready
            ? cubit.submitTitleForm
            : null,
        child: Text('Play'),
      ),
    );

    final displayNameForm = SimpleFormLayout(
      key: ValueKey(nextFormId()),
      title: const Text('Choose Username'),
      description: const Text(
        'This is the name that will be displayed to other players.',
      ),
      field: TextFormField(
        onChanged: (value) => cubit.updateDisplayName(value),
        initialValue: state.displayName,
        decoration: InputDecoration(
          labelText: 'Username',
          hintText: 'Baddest Gummy',
          errorText: state.displayNameMessage,
        ),
      ),
      primaryButton: ElevatedButton(
        onPressed: state.displayNameSubmission == SubmissionStatus.ready
            ? cubit.submitDisplayNameForm
            : null,
        child: Text('Join Game'),
      ),
      secondaryButton: TextButton(
        onPressed: state.displayNameSubmission == SubmissionStatus.ready
            ? cubit.unsubmitTitleForm
            : null,
        child: Text('Back'),
      ),
    );

    final pagedForm = PagedForm(
      controller: controller,
      squishy: true,
      backgroundColors: [
        Colors.white,
      ],
      forms: [
        titleForm,
        displayNameForm,
      ],
    );

    return MultiBlocListener(
      listeners: [
        BlocListener<JoinCubit, JoinState>(
          listenWhen: (previous, current) =>
              previous.currentPage != current.currentPage,
          listener: (context, state) =>
              controller.currentPage.value = state.currentPage,
        ),
        BlocListener<JoinCubit, JoinState>(
          listenWhen: (previous, current) =>
              current.popupErrorMessage != null &&
              previous.popupErrorMessage != current.popupErrorMessage,
          listener: (context, state) => ScaffoldMessenger.of(context)
              .showSnackBar(SnackBar(content: Text(state.popupErrorMessage!))),
        ),
      ],
      child: pagedForm,
    );
  }
}
