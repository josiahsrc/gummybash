import 'package:flutter/material.dart';
import 'package:liquid_swipe/liquid_swipe.dart';

class PagedFormController {
  PagedFormController({
    int initialPage = 0,
  }) : currentPage = ValueNotifier(initialPage);

  final ValueNotifier<int> currentPage;

  void nextPage() {
    currentPage.value++;
  }

  void previousPage() {
    currentPage.value--;
  }

  void dispose() {
    currentPage.dispose();
  }
}

class PagedForm extends StatefulWidget {
  const PagedForm({
    super.key,
    required this.controller,
    this.backgroundColors,
    required this.forms,
    required this.squishy,
  });

  final PagedFormController controller;
  final List<Color>? backgroundColors;
  final List<Widget> forms;
  final bool squishy;

  @override
  State<PagedForm> createState() => _PagedFormState();
}

class _PagedFormState extends State<PagedForm> {
  LiquidController? _liquid;
  PageController? _pageCtrl;

  @override
  void initState() {
    if (widget.squishy) {
      _liquid = LiquidController();
    } else {
      _pageCtrl = PageController();
    }

    widget.controller.currentPage.addListener(_onPageChangeCallback);
    super.initState();
  }

  @override
  void dispose() {
    widget.controller.currentPage.removeListener(_onPageChangeCallback);
    _pageCtrl?.dispose();
    super.dispose();
  }

  void _onPageChangeCallback() {
    final index = widget.controller.currentPage.value % widget.forms.length;

    _liquid?.animateToPage(
      page: index,
      duration: index < _liquid!.currentPage ? 300 : 500,
    );

    _pageCtrl?.animateToPage(
      index,
      duration: const Duration(milliseconds: 300),
      curve: Curves.easeInOut,
    );
  }

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    final colors = widget.backgroundColors ?? [theme.scaffoldBackgroundColor];

    final pages = List.generate(
      widget.forms.length,
      (index) => Container(
        color: colors[index % colors.length],
        child: widget.forms[index],
      ),
    );

    if (widget.squishy) {
      return LiquidSwipe(
        disableUserGesture: true,
        liquidController: _liquid,
        pages: pages,
      );
    } else {
      return PageView(
        physics: const NeverScrollableScrollPhysics(),
        controller: _pageCtrl,
        children: pages,
      );
    }
  }
}
