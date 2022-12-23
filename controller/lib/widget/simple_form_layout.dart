import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:controller/widget/intrinsic_scroller.dart';

import '../resource/theming.dart';

class SimpleFormLayout extends StatelessWidget {
  const SimpleFormLayout({
    Key? key,
    this.title,
    this.description,
    this.field,
    this.primaryButton,
    this.secondaryButton,
    this.actionBackgroundColor,
    this.extraBottomPolePadding = false,
  }) : super(key: key);

  final Widget? title;
  final Widget? description;
  final Widget? field;
  final Widget? primaryButton;
  final Widget? secondaryButton;
  final Color? actionBackgroundColor;
  final bool extraBottomPolePadding;

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
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

    final topInset = lerpDouble(mq.size.height * .1, 0, polePercent)!;
    final topPolePadding = mq.viewInsets.top + topInset;

    final content = IntrinsicScroller(
      padding: const EdgeInsets.all(Theming.edgeSpacing),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          SizedBox(height: topPolePadding),
          Spacer(flex: 4),
          if (title != null)
            DefaultTextStyle(
              style: theme.textTheme.headlineLarge!,
              child: title!,
            ),
          const SizedBox(height: 32),
          if (description != null)
            DefaultTextStyle(
              style: theme.textTheme.bodyMedium!,
              child: description!,
            ),
          const SizedBox(height: 16),
          if (field != null)
            ConstrainedBox(
              constraints: BoxConstraints(minHeight: 120),
              child: field!,
            ),
          const Spacer(flex: 5),
          SizedBox(height: 196.0 + bottomExtra),
        ],
      ),
    );

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
        Positioned.fill(child: content),
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
