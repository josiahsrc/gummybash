import 'package:flutter/material.dart';
// import 'package:flutter_joystick/flutter_joystick.dart';

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
            // Joystick(
            //   listener: (details) {},
            //   period: const Duration(milliseconds: 5),
            // ),
          ],
        ),
      ),
    );
  }
}
