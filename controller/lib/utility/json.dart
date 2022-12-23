
import 'package:flutter/material.dart';

DateTime? maybeDateTimeFromJson(dynamic json) {
  if (json == null) {
    return null;
  }
  return DateTime.parse(json as String);
}

String? maybeDateTimeToJson(DateTime? dateTime) {
  if (dateTime == null) {
    return null;
  }
  return dateTime.toIso8601String();
}

Color hexToColor(String code) {
  return Color(int.parse(code.substring(1, 9), radix: 16));
}

String colorToHex(Color color) {
  return '#${color.value.toRadixString(16)}';
}
