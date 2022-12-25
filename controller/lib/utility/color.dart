import 'package:flutter/material.dart';

extension ColorX on MaterialColor {
  Color shade(int shade) {
    int lower;
    int upper;
    if (shade < 100) {
      lower = 50;
      upper = 100;
    } else if (shade < 200) {
      lower = 100;
      upper = 200;
    } else if (shade < 300) {
      lower = 200;
      upper = 300;
    } else if (shade < 400) {
      lower = 300;
      upper = 400;
    } else if (shade < 500) {
      lower = 400;
      upper = 500;
    } else if (shade < 600) {
      lower = 500;
      upper = 600;
    } else if (shade < 700) {
      lower = 600;
      upper = 700;
    } else if (shade < 800) {
      lower = 700;
      upper = 800;
    } else if (shade < 900) {
      lower = 800;
      upper = 900;
    } else {
      lower = 900;
      upper = 900;
    }

    final t = (shade - lower) / (upper - lower);
    return Color.lerp(this[lower], this[upper], t)!;
  }
}

Color hexToColor(String code) {
  return Color(int.parse(code.substring(1, 9), radix: 16));
}

String colorToHex(Color color) {
  return '#${color.value.toRadixString(16)}';
}
