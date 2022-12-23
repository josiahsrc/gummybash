import 'package:flutter/material.dart';

import 'home_inner.dart';

class HomePage extends StatelessWidget {
  const HomePage({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: HomeInner(),
      ),
    );
  }
}
