#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""纯标准库合成轻快滑雪向循环 BGM（贴 Vivi 旁白：中低能量、无人声、中频留白）。"""

from __future__ import annotations

import math
import random
import struct
import wave
from pathlib import Path

SR = 44100
BPM = 100
BEAT = 60.0 / BPM
DURATION_SEC = 96  # 约 1.5 分钟，可循环
OUT = Path(__file__).resolve().parents[3] / "Assets" / "TestAudio" / "Bgm" / "bgm_vivi_ski_loop_v1.wav"

# C major 相关：C Am F G（偏轻松）
PROGRESSION = [
    (130.81, 164.81, 196.00),  # C
    (110.00, 130.81, 164.81),  # Am
    (87.31, 130.81, 174.61),   # F
    (98.00, 123.47, 146.83),   # G
]


def clamp(x: float, lo: float = -1.0, hi: float = 1.0) -> float:
    return lo if x < lo else hi if x > hi else x


def env_adsr(t: float, dur: float, a=0.02, d=0.08, s=0.7, r=0.12) -> float:
    if t < 0 or t > dur:
        return 0.0
    if t < a:
        return t / a
    if t < a + d:
        return 1.0 - (1.0 - s) * ((t - a) / d)
    if t < dur - r:
        return s
    if dur <= r:
        return s * max(0.0, 1.0 - t / dur)
    return s * max(0.0, (dur - t) / r)


def tone(freq: float, t: float, kind: str = "sine") -> float:
    phase = 2 * math.pi * freq * t
    if kind == "tri":
        # cheap triangle
        x = (phase / math.pi) % 2.0
        return 1.0 - abs(x - 1.0) * 2.0 if x < 1 else abs(x - 3.0) * -1  # fallback
    if kind == "soft_square":
        return math.tanh(3.0 * math.sin(phase))
    return math.sin(phase)


def tri(freq: float, t: float) -> float:
    # proper triangle via asin
    return (2 / math.pi) * math.asin(math.sin(2 * math.pi * freq * t))


def make_track() -> list[float]:
    n = int(SR * DURATION_SEC)
    buf = [0.0] * n
    rng = random.Random(42)

    bar = 4 * BEAT
    chord_len = 2 * bar  # 2 bars per chord

    # --- pads + soft arps ---
    t = 0.0
    chord_i = 0
    while t < DURATION_SEC:
        freqs = PROGRESSION[chord_i % len(PROGRESSION)]
        start = int(t * SR)
        length = int(chord_len * SR)
        for i in range(length):
            ti = i / SR
            abs_t = t + ti
            if start + i >= n:
                break
            e = env_adsr(ti, chord_len, a=0.15, d=0.2, s=0.55, r=0.35)
            # soft pad (low mix — leave mid for voice)
            pad = (
                0.18 * tone(freqs[0], abs_t)
                + 0.12 * tone(freqs[1], abs_t)
                + 0.10 * tone(freqs[2], abs_t)
            ) * e
            # sparkling soft arp (higher, quiet)
            step = int(ti / (BEAT / 2))
            arp_f = freqs[step % 3] * (2 if step % 4 != 3 else 1)
            arp = 0.07 * tri(arp_f, abs_t) * env_adsr(ti % (BEAT / 2), BEAT / 2, a=0.005, d=0.05, s=0.35, r=0.05)
            buf[start + i] += pad + arp
        t += chord_len
        chord_i += 1

    # --- light beat ---
    beat_i = 0
    while True:
        bt = beat_i * BEAT
        if bt >= DURATION_SEC:
            break
        start = int(bt * SR)
        # kick on 1 and 3
        if beat_i % 4 in (0, 2):
            for i in range(int(0.12 * SR)):
                if start + i >= n:
                    break
                ti = i / SR
                freq = 90 * math.exp(-18 * ti)
                buf[start + i] += 0.22 * math.sin(2 * math.pi * freq * ti) * math.exp(-10 * ti)
        # soft hat on offbeats
        if beat_i % 2 == 1:
            for i in range(int(0.04 * SR)):
                if start + i >= n:
                    break
                ti = i / SR
                noise = rng.uniform(-1, 1)
                buf[start + i] += 0.035 * noise * math.exp(-60 * ti)
        # light clap-ish on 2 and 4
        if beat_i % 4 in (1, 3):
            for i in range(int(0.06 * SR)):
                if start + i >= n:
                    break
                ti = i / SR
                noise = rng.uniform(-1, 1)
                buf[start + i] += 0.05 * noise * math.exp(-35 * ti)
        beat_i += 1

    # --- gentle high shimmer motif (sparse) ---
    motif = [523.25, 659.25, 783.99, 659.25]  # C5 E5 G5 E5
    m_t = 0.0
    mi = 0
    while m_t < DURATION_SEC:
        f = motif[mi % len(motif)]
        start = int(m_t * SR)
        note_dur = BEAT
        for i in range(int(note_dur * SR)):
            if start + i >= n:
                break
            ti = i / SR
            e = env_adsr(ti, note_dur, a=0.01, d=0.08, s=0.25, r=0.15)
            buf[start + i] += 0.045 * tone(f, (start + i) / SR, "soft_square") * e
        m_t += BEAT * 2
        mi += 1

    # normalize + soft limiter + edge fade for loop
    peak = max(abs(x) for x in buf) or 1.0
    target = 0.55
    gain = target / peak
    fade = int(0.25 * SR)
    for i, x in enumerate(buf):
        y = clamp(x * gain)
        if i < fade:
            y *= i / fade
        elif i > n - fade:
            y *= (n - i) / fade
        buf[i] = y
    return buf


def write_wav(path: Path, samples: list[float]) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    with wave.open(str(path), "w") as w:
        w.setnchannels(1)
        w.setsampwidth(2)
        w.setframerate(SR)
        frames = bytearray()
        for s in samples:
            v = int(clamp(s) * 32767)
            frames.extend(struct.pack("<h", v))
        w.writeframes(frames)


def main() -> None:
    print("synthesizing loop BGM...")
    samples = make_track()
    write_wav(OUT, samples)
    print(f"wrote {OUT} ({OUT.stat().st_size} bytes, {DURATION_SEC}s @ {SR}Hz)")


if __name__ == "__main__":
    main()
