# ANEXO 3 - FORMATOS XML VERSIÓN 1.1.0

Incluyen el aumento de 2 a 6 decimales en los campos de cantidad y precio unitario para quienes lo requieran. En el caso del formato de factura adicionalmente contiene información de retenciones de IVA presuntivo e Impuesto a la Renta que aplica para comercializadores de derivados de petróleo y retención presuntiva de IVA a los editores, distribuidores y voceadores que participan en la comercialización de periódicos y/o revistas.

## FORMATO XML FACTURA

| ETIQUETAS O TAGS | CARÁCTER | TIPO DE CAMPO | LONGITUD / FORMATO |
|------------------|----------|---------------|-------------------|
| `<?xml version="1.0" encoding="UTF-8" ?>` | Obligatorio | - | - |
| `<factura id="comprobante" version="1.1.0">` | Obligatorio | - | - |
| `<infoTributaria>` | Obligatorio | - | - |
| `<ambiente>1</ambiente>` | Obligatorio, conforme tabla 4 | Numérico | 1 |
| `<tipoEmision>1</tipoEmision>` | Obligatorio, conforme tabla 2 | Numérico | 1 |
| `<razonSocial>EMPRESA PUBLICA DE HIDROCARBUROS DEL ECUADOR EP PETROECUADOR</razonSocial>` | Obligatorio | Alfanumérico | Max 300 |
| `<nombreComercial>EMPRESA PUBLICA DE HIDROCARBUROS DEL ECUADOR EP PETROECUADOR</nombreComercial>` | Obligatorio cuando corresponda | Alfanumérico | Max 300 |
| `<ruc>1768153530001</ruc>` | Obligatorio | Numérico | 13 |
| `<claveAcceso>0403201301176815353000110015010000000081234567816</claveAcceso>` | Obligatorio, conforme tabla 1 | Numérico | 49 |
| `<codDoc>01</codDoc>` | Obligatorio, conforme tabla 3 | Numérico | 2 |
| `<estab>001</estab>` | Obligatorio | Numérico | 3 |
| `<ptoEmi>501</ptoEmi>` | Obligatorio | Numérico | 3 |
| `<secuencial>000000008</secuencial>` | Obligatorio | Numérico | 9 |
| `<dirMatriz>Alpallana</dirMatriz>` | Obligatorio | Alfanumérico | Max 300 |
| `</infoTributaria>` | Obligatorio | - | - |
| `<infoFactura>` | Obligatorio | - | - |
| `<fechaEmision>04/03/2013</fechaEmision>` | Obligatorio | Fecha | dd/mm/aaaa |
| `<dirEstablecimiento>Alpallana</dirEstablecimiento>` | Obligatorio cuando corresponda | Alfanumérico | Max 300 |
| `<contribuyenteEspecial>5368</contribuyenteEspecial>` | Obligatorio cuando corresponda | Alfanumérico | Min 3 Max 13 |
| `<obligadoContabilidad>SI</obligadoContabilidad>` | Obligatorio cuando corresponda | Texto | SI / NO |
| `<tipoIdentificacionComprador>04</tipoIdentificacionComprador>` | Obligatorio, conforme tabla 6 | Numérico | 2 |
| `<guiaRemision>001-001-000000001</guiaRemision>` | Obligatorio cuando corresponda | Numérico | 15 |
| `<razonSocialComprador>PRUEBAS SERVICIO DERENTAS INTERNAS</razonSocialComprador>` | Obligatorio | Alfanumérico | Max 300 |
| `<identificacionComprador>1760013210001</identificacionComprador>` | Obligatorio | Alfanumérico | Max 20 |
| `<direccionComprador>salinas y santiago</direccionComprador>` | Obligatorio, cuando corresponda | Alfanumérico | Max 300 |
| `<totalSinImpuestos>64.94</totalSinImpuestos>` | Obligatorio | Numérico | Max 14 |
| `<totalDescuento>5.00</totalDescuento>` | Obligatorio | Numérico | Max 14 |
| `<totalConImpuestos>` | Obligatorio | - | - |
| `<totalImpuesto>` | Obligatorio | - | - |
| `<codigo>2</codigo>` | Obligatorio, conforme tabla 16 | Numérico | 1 |
| `<codigoPorcentaje>2</codigoPorcentaje>` | Obligatorio, conforme tabla 17 | Numérico | Min 1 Max 4 |
| `<descuentoAdicional>5.00</descuentoAdicional>` | Opcional, aplica para código impuesto 2. | Numérico | Max 14 |
| `<baseImponible>68.19</baseImponible>` | Obligatorio | Numérico | Max 14 |
| `<valor>7.58</valor>` | Obligatorio | Numérico | Max 14 |
| `</totalImpuesto>` | Obligatorio | - | - |
| `<totalImpuesto>` | Obligatorio | - | - |
| `<codigo>3</codigo>` | Obligatorio, conforme tabla 16 | Numérico | 1 |
| `<codigoPorcentaje>3072</codigoPorcentaje>` | Obligatorio, conforme tabla 18 | Numérico | Min 1 Max 4 |
| `<baseImponible>64.94</baseImponible>` | Obligatorio | Numérico | Max 14 |
| `<valor>3.25</valor>` | Obligatorio | Numérico | Max 14 |
| `</totalImpuesto>` | Obligatorio | - | - |
| `</totalConImpuestos>` | Obligatorio | - | - |
| `<propina>0.00</propina>` | Obligatorio | Numérico | Max 14 |
| `<importeTotal>73.09</importeTotal>` | Obligatorio | Numérico | Max 14 |
| `<moneda>DOLAR</moneda>` | Obligatorio cuando corresponda | Alfanumérico | Max 15 |
| `<pagos>` | Obligatorio | - | - |
| `<pago>` | Obligatorio | - | - |
| `<formaPago>21</formaPago>` | Obligatorio, conforme tabla 24 | Numérico | 2 |
| `<total>73,09</total>` | Obligatorio | Numérico | Max 14 |
| `<plazo>60<plazo>` | Obligatorio, cuando corresponda | Numérico | Max 14 |
| `<unidadTiempo>dias</unidadTiempo>` | Obligatorio, cuando corresponda | Texto | Max 10 |
| `</pago>` | Obligatorio | - | - |
| `</pagos>` | Obligatorio | - | - |
| `<valorRetIva>0.00</valorRetIva>` | Opcional | Numérico | Max 14 |
| `<valorRetRenta>0.00</valorRetRenta>` | Opcional | Numérico | Max 14 |
| `</infoFactura>` | Obligatorio | - | - |
| `<detalles>` | Obligatorio | - | - |
| `<detalle>` | Obligatorio | - | - |
| `<codigoPrincipal>125BJC-01</codigoPrincipal>` | Obligatorio | Alfanumérico | Max 25 |
| `<codigoAuxiliar>1234D56789-A</codigoAuxiliar>` | Obligatorio cuando corresponda | Alfanumérico | Max 25 |
| `<descripcion>DERIVADOS PETRÓLEO</descripcion>` | Obligatorio | Alfanumérico | Max 300 |
| `<cantidad>2.542563</cantidad>` | Obligatorio | Numérico | Max 18, hasta 6 decimales |
| `<precioUnitario>25.542365</precioUnitario>` | Obligatorio | Numérico | Max 18, hasta 6 decimales |
| `<descuento>0.00</descuento>` | Obligatorio | Numérico | Max 14 |
| `<precioTotalSinImpuesto>64.94</precioTotalSinImpuesto>` | Obligatorio | Numérico | Max 14 |
| `<detallesAdicionales>` | Obligatorio cuando corresponda | - | - |
| `<detAdicional nombre="ABCD" valor="EFGH"/>` | Obligatorio cuando corresponda | Alfanumérico | Max 300 |
| `<detAdicional nombre="ABCD " valor="EFGH"/>` | Obligatorio cuando corresponda | Alfanumérico | Max 300 |
| `<detAdicional nombre="ABCD" valor="EFGH"/>` | Obligatorio cuando corresponda | Alfanumérico | Max 300 |
| `</detallesAdicionales>` | Obligatorio cuando corresponda | - | - |
| `<impuestos>` | Obligatorio | - | - |
| `<impuesto>` | Obligatorio | - | - |
| `<codigo>2</codigo>` | Obligatorio, conforme tabla 16 | Numérico | 1 |
| `<codigoPorcentaje>2</codigoPorcentaje>` | Obligatorio, conforme tabla 17 | Numérico | Min 1 Max 4 |
| `<tarifa>12</tarifa>` | Obligatorio | Numérico | Min 1 Max 4 / 2 enteros, 2 decimales |
| `<baseImponible>68.19</baseImponible>` | Obligatorio | Numérico | Max 14 |
| `<valor>8.18</valor>` | Obligatorio | Numérico | Max 14 |
| `</impuesto>` | Obligatorio | - | - |
| `<impuesto>` | Obligatorio | - | - |
| `<codigo>3</codigo>` | Obligatorio, conforme tabla 16 | Numérico | 1 |
| `<codigoPorcentaje>3072</codigoPorcentaje>` | Obligatorio, conforme tabla 18 | Numérico | Min 1 Max 4 |
| `<tarifa>5</tarifa>` | Obligatorio | Numérico | Min 1 Max 4 |
| `<baseImponible>64.94</baseImponible>` | Obligatorio | Numérico | Max 14 |
| `<valor>3.25</valor>` | Obligatorio | Numérico | Max 14 |
| `</impuesto>` | Obligatorio | - | - |
| `</impuestos>` | Obligatorio | - | - |
| `</detalle>` | Obligatorio | - | - |
| `</detalles>` | Obligatorio | - | - |
| `<retenciones>` | Obligatorio cuando corresponda | - | - |
| `<retencion>` | Obligatorio cuando corresponda | - | - |
| `<codigo>4</codigo>` | Obligatorio cuando corresponda, conforme tabla 22 | Numérico | 1 |
| `<codigoPorcentaje>327</codigoPorcentaje>` | Obligatorio cuando corresponda, conforme tabla 23 | Numérico | Min 1 Max 3 |
| `<tarifa>0.20</tarifa>` | Obligatorio cuando corresponda | Numérico | Min 1 Max 5 / 3 enteros, dos decimales |
| `<valor>0.13</valor>` | Obligatorio cuando corresponda | Numérico | Max 14 / 12 enteros, 2 decimales |
| `</retencion>` | Obligatorio cuando corresponda | - | - |
| `<retencion>` | Obligatorio cuando corresponda | - | - |
| `<codigo>4</codigo>` | Obligatorio cuando corresponda, conforme tabla 22 | Numérico | 1 |
| `<codigoPorcentaje>328</codigoPorcentaje>` | Obligatorio cuando corresponda, conforme tabla 23 | Numérico | Min 1 Max 3 |
| `<tarifa>0.30</tarifa>` | Obligatorio cuando corresponda | Numérico | Min 1 Max 5 / 3 enteros, dos decimales |
| `<valor>0.19</valor>` | Obligatorio cuando corresponda | Numérico | Max 14 / 12 enteros, 2 decimales |
| `</retencion>` | Obligatorio cuando corresponda | - | - |
| `<retencion>` | Obligatorio cuando corresponda | - | - |
| `<codigo>4</codigo>` | Obligatorio cuando corresponda, conforme tabla 22 | Numérico | 1 |
| `<codigoPorcentaje>3</codigoPorcentaje>` | Obligatorio cuando corresponda, conforme tabla 23 | Numérico | Min 1 Max 3 |
| `<tarifa>1</tarifa>` | Obligatorio cuando corresponda | Numérico | Min 1 Max 5 / 3 enteros, dos decimales |
| `<valor>2.00</valor>` | Obligatorio cuando corresponda | Numérico | Max 14 / 12 enteros, 2 decimales |
| `</retencion>` | Obligatorio cuando corresponda | - | - |
| `</retenciones>` | Obligatorio cuando corresponda | - | - |
| `<infoAdicional>` | Obligatorio cuando corresponda | - | - |
| `<campoAdicional nombre="Codigo Impuesto ISD">4580</campoAdicional>` | Obligatorio cuando corresponda | Alfanumérico | Max 300 |
| `<campoAdicional nombre="Impuesto ISD">15.42x</campoAdicional>` | Obligatorio cuando corresponda | Alfanumérico | Max 300 |
| `</infoAdicional>` | Obligatorio cuando corresponda | - | - |
| `</factura>` | Obligatorio | - | - |