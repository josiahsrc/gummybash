import 'package:controller/model/model.dart';
import 'package:controller/page/play/bloc/play_cubit.dart';
import 'package:controller/widget/joystick.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

class PlayInner extends StatelessWidget {
  const PlayInner({
    super.key,
  });

  @override
  Widget build(BuildContext context) {
    final playCubit = context.watch<PlayCubit>();

    final controls = Row(
      children: [
        Expanded(
          child: Padding(
            padding: const EdgeInsets.all(24.0),
            child: Joystick(
              onMove: (delta) => playCubit.updateControls(
                joystickX: delta.dx,
                joystickY: -delta.dy,
              ),
            ),
          ),
        ),
        Expanded(
          child: Padding(
            padding: const EdgeInsets.all(32.0),
            child: Center(
              child: AspectRatio(
                aspectRatio: 1,
                child: ClipRRect(
                  borderRadius: BorderRadius.circular(9999),
                  child: Material(
                    color: Colors.grey,
                    child: InkWell(
                      onTap: () => playCubit.updateControls(isShooting: true),
                      child: Container(
                        decoration: BoxDecoration(
                          shape: BoxShape.circle,
                        ),
                        child: Center(
                          child: Icon(
                            Icons.add,
                            size: 64,
                            color: Colors.white,
                          ),
                        ),
                      ),
                    ),
                  ),
                ),
              ),
            ),
          ),
        ),
      ],
    );

    Widget main;
    if (playCubit.state.winner != UserType.none) {
      final msg = playCubit.state.winner == UserType.gingerBreadHouse
          ? 'House\nWins!'
          : 'Bears\nWin!';
      main = Center(
        child: Text(
          msg,
          style: Theme.of(context).textTheme.headline4,
        ),
      );
    } else {
      Widget image;
      if (playCubit.state.type == UserType.gingerBreadHouse) {
        image = Image.asset(
          'assets/house.png',
          width: MediaQuery.of(context).size.width * 0.8,
        );
      } else if (playCubit.state.type == UserType.gummyBear) {
        image = Image.asset(
          'assets/bear.png',
          width: MediaQuery.of(context).size.width * 0.8,
        );

        // apply the color to the image
        image = ColorFiltered(
          colorFilter: ColorFilter.mode(
            playCubit.state.color?? Colors.white,
            BlendMode.srcATop,
          ),
          child: image,
        );
      } else {
        image = Container();
      }

      main = Center(child: image);
    }

    final content = Column(
      children: [
        SizedBox(
          height: 124,
          child: Center(
            child: Text(
              playCubit.state.timeRemaining ?? '',
              style: Theme.of(context).textTheme.headline4,
            ),
          ),
        ),
        Expanded(
          flex: 3,
          child: main,
        ),
        Divider(
          thickness: 5,
        ),
        Expanded(
          flex: 2,
          child: controls,
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
