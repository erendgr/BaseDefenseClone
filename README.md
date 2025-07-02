# Projenin Özellikleri

## Sinyal Sistemi
> Oyunda bağımlılıkları azaltmak ve modülerliği artırmak amacıyla, **loosely coupled** bir iletişim yapısı sağlayan **Observer Pattern** tabanlı bir sinyal sistemi uygulandı.
Bu sistem, farklı bileşenlerin olayları dinleyip birbirinden bağımsız şekilde tepki vermesine olanak tanır.
Ayrıca, **Single Responsibility Principle (SOLID)** kapsamında bileşenler sadece kendi görevlerine odaklanırken, sinyal sistemi bileşenler arasındaki bağımlılığı minimize eder.
---

## Generic State Machine Sistemi
> AI karakterlerin davranışlarını modüler ve esnek yönetmek için **generic** olarak tasarlanmış bir state machine yapısı kullanıldı.
Bu yapı, **State Pattern** prensibine uygundur ve farklı durumlar (state) nesneleri arasında geçişi kolaylaştırır.
Ayrıca, **Single Responsibility Principle (SOLID)** kapsamında her state sınıfı sadece kendi davranışından sorumludur.
Bu sayede, kodun okunabilirliği, bakımı ve genişletilebilirliği artar, yeni davranışlar kolayca eklenebilir.
---

## Object Pooling Sistemi
> Sistemdeki **garbage collection** yükünü azaltmak için object pooling pattern kullanıldı.  
Bu sistem, **Open/Closed prensibine** uygun şekilde tasarlandı.  
Bu sayede modüler ve yeni nesne türlerine kolayca genişletilebilir bir yapı elde edildi.
---

## Veri Yönetim Sistemi
> Oyundaki tüm veriler, **ScriptableObject** tabanlı bir veri yönetim sistemi ile organize edildi.  
Bu sistem, veri odaklı mimariyi destekleyerek sahneye bağımlı olmadan runtime veri kullanımını mümkün kılar.  
Veriler, **Single Responsibility** ve **Open/Closed prensiplerine** uygun olarak modüler alt sınıflara ayrıldı.  
Bu sayede sistem, hem editor üzerinden kolayca düzenlenebilir hem de yeni veri tipleri için sürdürülebilir ve genişletilebilir hale getirildi.
---

## Save Sistemi
> Oyundaki tüm veri kayıt işlemleri için **Easy Save 3** eklentisi kullanıldı.  
Sistem, **persistent data** yönetimini sadeleştirirken, özelleştirilebilir veri formatları sayesinde modüler ve genişletilebilir bir yapı sağladı.
---

## Kamera
> Kamera sistemi olarak Unity’nin güçlü **Cinemachine** kütüphanesi tercih edildi.
---

## Animasyon
> Animasyon işlemleri için **DOTween Pro** kullanıldı.  
Tween akışlarıyla birlikte UI ve obje animasyonları performanslı ve kontrol edilebilir hale getirildi.
---

## UI Tasarım
> Kullanıcı arayüzü tasarımlarında **GUI Mobile Hyper-Casual** ve **GUI PRO Kit – Casual Game** paketleri kullanıldı.  
Bu sayede arayüzler mobil odaklı, sade ve kullanıcı dostu bir yapıya kavuştu.
---

## Polish
> Projenin görsel kalitesini artırmak için **BOXOPHOBIC (Skybox)**, **Toony Colors Pro 2** ve **MK Toon – Stylized Shader** kullanıldı.
---
