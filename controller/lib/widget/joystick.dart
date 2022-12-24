import 'package:flutter/material.dart';
import 'package:simple_animations/simple_animations.dart';

class Joystick extends StatefulWidget {
  const Joystick({
    super.key,
    this.onMove,
  });

  final void Function(Offset delta)? onMove;

  @override
  State<Joystick> createState() => _JoystickState();
}

class _JoystickState extends State<Joystick> with AnimationMixin {
  late Animation<double> t;
  Offset targetDelta = Offset.zero;

  @override
  void initState() {
    t = Tween<double>(begin: 0.0, end: 1.0).animate(controller);
    controller.duration = const Duration(milliseconds: 50);
    controller.addListener(onChangeAnimation);
    super.initState();
  }

  void onChangeAnimation() {
    setState(() {});
  }

  Widget _build(
    BuildContext context,
    BoxConstraints constraints,
  ) {
    final size = constraints.biggest;
    final radius = size.width - 24;

    final bg = Container(
      decoration: BoxDecoration(
        border: Border.all(
          color: Colors.black45,
          width: 4,
        ),
        shape: BoxShape.circle,
      ),
    );

    final stick = Transform.translate(
      offset: Offset.lerp(
        Offset.zero,
        targetDelta,
        Curves.easeInOut.transform(t.value),
      )!,
      child: Center(
        child: Container(
          width: radius,
          height: radius,
          decoration: BoxDecoration(
            shape: BoxShape.circle,
            color: Colors.grey,
          ),
        ),
      ),
    );

    final content = Stack(
      fit: StackFit.expand,
      children: [
        bg,
        stick,
      ],
    );

    Offset getActualDelta(Offset pos) {
      final center = Offset(size.width / 2, size.height / 2);
      final offset = pos - center;

      var retOffset = offset;
      final maxOffset = radius / 12;
      if (retOffset.distance > maxOffset) {
        retOffset = retOffset * (maxOffset / retOffset.distance);
      }

      return retOffset;
    }

    return GestureDetector(
      onPanDown: (details) {
        targetDelta = getActualDelta(details.localPosition);
        setState(() {});
        controller.forward(from: t.value);
        widget.onMove?.call(targetDelta);
      },
      onPanStart: (details) {
        targetDelta = getActualDelta(details.localPosition);
        setState(() {});
        controller.forward(from: t.value);
        widget.onMove?.call(targetDelta);
      },
      onPanUpdate: (details) {
        targetDelta = getActualDelta(details.localPosition);
        setState(() {});
        widget.onMove?.call(targetDelta);
      },
      onPanEnd: (details) {
        setState(() {});
        controller.reverse(from: t.value);
        widget.onMove?.call(Offset.zero);
      },
      onPanCancel: () {
        setState(() {});
        controller.reverse(from: t.value);
        widget.onMove?.call(Offset.zero);
      },
      child: content,
    );
  }

  @override
  Widget build(BuildContext context) {
    return AspectRatio(
      aspectRatio: 1,
      child: LayoutBuilder(builder: _build),
    );
  }
}
