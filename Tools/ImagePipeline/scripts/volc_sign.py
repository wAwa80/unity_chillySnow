# -*- coding: utf-8 -*-
"""
火山引擎 OpenAPI SigV4（Header 场景）。

规范：
  https://docs.volcengine.com/docs/6369/67268?lang=zh
  https://www.volcengine.com/docs/6369/67269

本模块只产出 Header 鉴权（Authorization / X-Date / X-Content-Sha256 等），
不实现 Query 签名场景（X-Algorithm / X-Credential / X-Signature）。
"""

from __future__ import annotations

import datetime
import hashlib
import hmac
from typing import Dict, Mapping, Optional
from urllib.parse import quote


def _hmac_sha256(key: bytes, content: str) -> bytes:
    return hmac.new(key, content.encode("utf-8"), hashlib.sha256).digest()


def _hash_sha256(content: str) -> str:
    return hashlib.sha256(content.encode("utf-8")).hexdigest()


def _norm_query(params: Mapping[str, str]) -> str:
    """规范化 Query：按 key 排序，RFC3986 编码。"""
    parts = []
    for key in sorted(params.keys()):
        k = quote(str(key), safe="-_.~")
        v = quote(str(params[key]), safe="-_.~")
        parts.append(f"{k}={v}")
    return "&".join(parts).replace("+", "%20")


def utc_now() -> datetime.datetime:
    return datetime.datetime.now(datetime.timezone.utc)


def sign_headers(
    *,
    method: str,
    host: str,
    path: str,
    query: Mapping[str, str],
    body: str,
    access_key: str,
    secret_key: str,
    region: str,
    service: str,
    content_type: str = "application/json",
    security_token: str = "",
    now: Optional[datetime.datetime] = None,
) -> Dict[str, str]:
    """
    计算并返回应附加到 HTTP 请求的签名 Header。

    返回字段：
      Host, Content-Type, X-Date, X-Content-Sha256, Authorization
      （若传入 security_token，另含 X-Security-Token）
    """
    if not access_key or not secret_key:
        raise ValueError("access_key / secret_key 不能为空")

    when = now or utc_now()
    x_date = when.strftime("%Y%m%dT%H%M%SZ")
    short_date = x_date[:8]
    body_str = body if body is not None else ""
    payload_hash = _hash_sha256(body_str)

    signed_headers = ["content-type", "host", "x-content-sha256", "x-date"]
    canonical_headers = "\n".join(
        [
            f"content-type:{content_type}",
            f"host:{host}",
            f"x-content-sha256:{payload_hash}",
            f"x-date:{x_date}",
        ]
    )
    if security_token:
        # STS 临时凭证：头参与签名
        signed_headers.append("x-security-token")
        canonical_headers += f"\nx-security-token:{security_token}"

    signed_headers_str = ";".join(signed_headers)
    canonical_request = "\n".join(
        [
            method.upper(),
            path or "/",
            _norm_query(dict(query)),
            canonical_headers,
            "",
            signed_headers_str,
            payload_hash,
        ]
    )
    hashed_canonical = _hash_sha256(canonical_request)
    credential_scope = f"{short_date}/{region}/{service}/request"
    string_to_sign = "\n".join(
        ["HMAC-SHA256", x_date, credential_scope, hashed_canonical]
    )

    k_date = _hmac_sha256(secret_key.encode("utf-8"), short_date)
    k_region = _hmac_sha256(k_date, region)
    k_service = _hmac_sha256(k_region, service)
    k_signing = _hmac_sha256(k_service, "request")
    signature = _hmac_sha256(k_signing, string_to_sign).hex()

    authorization = (
        f"HMAC-SHA256 Credential={access_key}/{credential_scope}, "
        f"SignedHeaders={signed_headers_str}, Signature={signature}"
    )

    headers: Dict[str, str] = {
        "Host": host,
        "Content-Type": content_type,
        "X-Date": x_date,
        "X-Content-Sha256": payload_hash,
        "Authorization": authorization,
    }
    if security_token:
        headers["X-Security-Token"] = security_token
    return headers
