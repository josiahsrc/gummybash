import 'package:flutter/material.dart';

class {{name.pascalCase()}}Inner extends StatelessWidget {
  const {{name.pascalCase()}}Inner({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return CustomScrollView(
      slivers: [
        SliverAppBar(
          title: const Text('{{name.titleCase()}}'),
        ),
      ],
    );
  }
}
