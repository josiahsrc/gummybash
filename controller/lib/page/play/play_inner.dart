import 'package:flutter/material.dart';

class PlayInner extends StatelessWidget {
  const PlayInner({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    final content = CustomScrollView(
      slivers: [
        SliverAppBar(
          title: const Text('Play'),
        ),
      ],
    );

    return Scaffold(
      body: SafeArea(
        child: content,
      ),
    );
  }
}
