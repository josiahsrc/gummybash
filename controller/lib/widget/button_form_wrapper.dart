import 'dart:ui';

import 'package:flutter/material.dart';

import '../resource/theming.dart';

class ButtonFormWrapper extends StatelessWidget {
  const ButtonFormWrapper({
    Key? key,
    this.primaryButton,
    this.secondaryButton,
    this.actionBackgroundColor,
    required this.child,
    this.extraBottomPolePadding = false,
  }) : super(key: key);

  final Widget? primaryButton;
  final Widget? secondaryButton;
  final Color? actionBackgroundColor;
  final bool extraBottomPolePadding;
  final Widget child;

  @override
  Widget build(BuildContext context) {
    final mq = MediaQuery.of(context);

    final viewInsets = EdgeInsets.fromWindowPadding(
      WidgetsBinding.instance.window.viewInsets,
      WidgetsBinding.instance.window.devicePixelRatio,
    );

    final polePadding =
        primaryButton != null && secondaryButton != null ? 42 : 84;
    final polePercent = (viewInsets.bottom / 32).clamp(0, 1).toDouble();

    final bottomExtra = extraBottomPolePadding ? 24 : 0;
    final bottomInset = lerpDouble(polePadding, 0, polePercent)!;
    final bottomPolePadding = mq.viewInsets.bottom + bottomInset + bottomExtra;

    final actions = Container(
      padding: EdgeInsets.symmetric(
        horizontal: Theming.edgeSpacingMore,
        vertical: Theming.edgeSpacing,
      ),
      decoration: BoxDecoration(
        gradient: actionBackgroundColor != null
            ? LinearGradient(
                begin: Alignment(0, -.75),
                end: Alignment.topCenter,
                colors: [
                  actionBackgroundColor!,
                  actionBackgroundColor!.withOpacity(0),
                ],
              )
            : null,
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          if (primaryButton != null) primaryButton!,
          if (secondaryButton != null && primaryButton != null)
            const SizedBox(height: 8),
          if (secondaryButton != null) secondaryButton!,
          SizedBox(height: bottomPolePadding),
        ],
      ),
    );

    return Stack(
      children: [
        Positioned.fill(child: child),
        Positioned(
          bottom: 0,
          left: 0,
          right: 0,
          child: actions,
        ),
      ],
    );
  }
}
