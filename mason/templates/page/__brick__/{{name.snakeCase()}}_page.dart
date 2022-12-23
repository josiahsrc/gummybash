import 'package:flutter/material.dart';

import '{{name.snakeCase()}}_inner.dart';

class {{name.pascalCase()}}Page extends StatelessWidget {
  const {{name.pascalCase()}}Page({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: {{name.pascalCase()}}Inner(),
      ),
    );
  }
}
