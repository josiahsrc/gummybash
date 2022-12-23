import 'package:controller/widget/joystick.dart';
import 'package:flutter/material.dart';

class PlayInner extends StatelessWidget {
  const PlayInner({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: Row(
          children: [
            Expanded(
              child: Joystick(),
            ),
          ],
        ),
      ),
    );
  }
}
