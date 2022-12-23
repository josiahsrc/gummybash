import 'package:flutter/material.dart';

class HomeInner extends StatelessWidget {
  const HomeInner({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return CustomScrollView(
      slivers: [
        SliverAppBar(
          title: const Text('Home'),
        ),
      ],
    );
  }
}
